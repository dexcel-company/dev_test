// React + Vite + TypeScript frontend for GOLDx Token Swap (Styled UI w/ external CSS)

import { useState, useEffect } from "react";
import { ethers, BrowserProvider, Contract, JsonRpcSigner } from "ethers";
import "./App.css";

// Network configurations
const networks = {
  ethereumSepolia: {
    chainId: "0xaa36a7",
    chainName: "Sepolia",
    nativeCurrency: {
      name: "Sepolia Ether",
      symbol: "ETH",
      decimals: 18,
    },
    rpcUrls: ["https://rpc.sepolia.org"],
    blockExplorerUrls: ["https://sepolia.etherscan.io/"],
  },
  arbitrumSepolia: {
    chainId: "0x66eee",
    chainName: "Arbitrum Sepolia",
    nativeCurrency: {
      name: "Ether",
      symbol: "ETH",
      decimals: 18,
    },
    rpcUrls: ["https://sepolia-rollup.arbitrum.io/rpc"],
    blockExplorerUrls: ["https://sepolia.arbiscan.io/"],
  },
};

// Cross-chain destination IDs
const crossChainDestinations = {
  polygonAmoy: 40267,
  ethereumSepolia: 40161,
  arbitrumSepolia: 40231,
};

const tokenAddresses: Record<string, Record<string, string>> = {
  polygonAmoy: {
    USDC: import.meta.env.VITE_USDC_ADDRESS_AMOY,
    GLD: import.meta.env.VITE_GLD_ADDRESS_AMOY,
  },
  ethereumSepolia: {
    USDC: import.meta.env.VITE_USDC_ADDRESS_SEPOLIA,
    GLD: import.meta.env.VITE_GLD_ADDRESS_SEPOLIA,
  },
  arbitrumSepolia: {
    USDC: import.meta.env.VITE_USDC_ADDRESS_ARBITRUM_SEPOLIA,
    GLD: import.meta.env.VITE_GLD_ADDRESS_ARBITRUM_SEPOLIA,
  },
};

const tokenABI = [
  "function approve(address spender, uint256 amount) external returns (bool)",
  "function decimals() view returns (uint8)",
  "function balanceOf(address owner) view returns (uint256)"
];

const swapContractAddresses: Record<string, string> = {
  polygonAmoy: import.meta.env.VITE_SWAP_CONTRACT_ADDRESS_AMOY,
  ethereumSepolia: import.meta.env.VITE_SWAP_CONTRACT_ADDRESS_SEPOLIA,
  arbitrumSepolia: import.meta.env.VITE_SWAP_CONTRACT_ADDRESS_ARBITRUM_SEPOLIA,
};

const swapABI = [
  "function buy(address inputToken, uint256 amount) external",
  "function buyCrossChain(address token, uint256 amount, uint32 dstEid, address toOnDst) external payable",
  "function sell(address outputToken, uint256 goldAmount) external"
];

const oracleAddresses: Record<string, string> = {
  polygonAmoy: import.meta.env.VITE_ORACLE_CONTRACT_ADDRESS_AMOY,
  ethereumSepolia: import.meta.env.VITE_ORACLE_CONTRACT_ADDRESS_SEPOLIA,
  arbitrumSepolia: import.meta.env.VITE_ORACLE_CONTRACT_ADDRESS_ARBITRUM_SEPOLIA,
};

const oracleABI = [
  "function getPrice() view returns (uint256)"
];

const Tabs = ({ tab, setTab }: { tab: "buy" | "sell"; setTab: (tab: "buy" | "sell") => void }) => (
  <div className="tab-buttons">
    <button
      className={tab === "buy" ? "tab active" : "tab"}
      onClick={() => setTab("buy")}
      type="button"
    >
      Buy
    </button>
    <button
      className={tab === "sell" ? "tab active" : "tab"}
      onClick={() => setTab("sell")}
      type="button"
    >
      Sell
    </button>
  </div>
);

function App() {
  const [provider, setProvider] = useState<BrowserProvider | null>(null);
  const [signer, setSigner] = useState<JsonRpcSigner | null>(null);
  const [address, setAddress] = useState<string>("");
  const [token, setToken] = useState<string>("USDC");
  const [amount, setAmount] = useState<string>("0");
  const [status, setStatus] = useState<string>("");
  const [goldPrice, setGoldPrice] = useState<string>("");
  const [tab, setTab] = useState<"buy" | "sell">("buy");
  const [balance, setBalance] = useState<string>("");
  const [selectedNetwork, setSelectedNetwork] = useState<string>("ethereumSepolia");
  const [targetNetwork, setTargetNetwork] = useState<string>("arbitrumSepolia");
  const [isLoadingData, setIsLoadingData] = useState<boolean>(false);

  const numericBalance = balance && !isNaN(Number(balance)) ? Number(balance) : 0;
  const numericAmount = amount && !isNaN(Number(amount)) ? Number(amount) : 0;
  const clampedAmount = Math.max(0, Math.min(numericAmount, numericBalance));

  useEffect(() => {
    const interval = setInterval(() => {
      if (provider) fetchGoldPrice(provider);
    }, 10000);
    return () => clearInterval(interval);
  }, [provider]);

  // Слушаем события смены сети в MetaMask
  useEffect(() => {
    if (!window.ethereum) return;

    const handleChainChanged = async () => {
      if (!window.ethereum) return;

      // Пересоздаем провайдер и синер при смене сети
      const newProvider = new ethers.BrowserProvider(window.ethereum);
      const newSigner = await newProvider.getSigner();

      // Получаем текущую сеть и синхронизируем выбранную сеть
      const network = await newProvider.getNetwork();
      const chainId = network.chainId.toString(16);

      // Определяем, какая сеть выбрана
      if (chainId === "aa36a7") {
        setSelectedNetwork("ethereumSepolia");
      } else if (chainId === "66eee") {
        setSelectedNetwork("arbitrumSepolia");
      }

      setProvider(newProvider);
      setSigner(newSigner);

      // Обновляем данные
      await fetchGoldPrice(newProvider);
      await fetchBalance();
    };

    const handleAccountsChanged = async (accounts: unknown) => {
      const accountArray = accounts as string[];
      if (accountArray.length === 0) {
        // Пользователь отключил кошелек
        setAddress("");
        setProvider(null);
        setSigner(null);
      } else {
        // Пользователь сменил аккаунт
        setAddress(accountArray[0]);
        if (provider) {
          const newSigner = await provider.getSigner();
          setSigner(newSigner);
          await fetchBalance();
        }
      }
    };

    window.ethereum.on('chainChanged', handleChainChanged);
    window.ethereum.on('accountsChanged', handleAccountsChanged);

    return () => {
      if (window.ethereum) {
        window.ethereum.removeListener('chainChanged', handleChainChanged);
        window.ethereum.removeListener('accountsChanged', handleAccountsChanged);
      }
    };
  }, []);

  const fetchBalance = async () => {
    if (!signer || !address) return;
    try {
      const tokenKey = tab === "sell" ? "GLD" : token;
      const tokenAddress = tokenAddresses[selectedNetwork][tokenKey];
      const erc20 = new Contract(tokenAddress, tokenABI, signer);
      const decimals: number = await erc20.decimals();
      const bal: bigint = await erc20.balanceOf(address);
      setBalance(ethers.formatUnits(bal, decimals));
    } catch (e) {
      setBalance("");
    }
  };

  useEffect(() => {
    fetchBalance();
  }, [signer, address, token, tab, selectedNetwork]);

  // Обновляем цену золота при смене сети
  useEffect(() => {
    if (provider) {
      fetchGoldPrice(provider);
    }
  }, [selectedNetwork, provider]);

  const connectWallet = async () => {
    if (!window.ethereum) return alert("Install MetaMask");
    const browserProvider = new ethers.BrowserProvider(window.ethereum);
    const signer = await browserProvider.getSigner();
    const addr = await signer.getAddress();

    // Получаем текущую сеть и синхронизируем выбранную сеть
    const network = await browserProvider.getNetwork();
    const chainId = network.chainId.toString(16);

    // Определяем, какая сеть выбрана
    if (chainId === "aa36a7") {
      setSelectedNetwork("ethereumSepolia");
    } else if (chainId === "66eee") {
      setSelectedNetwork("arbitrumSepolia");
    }

    await fetchGoldPrice(browserProvider);
    setProvider(browserProvider);
    setSigner(signer);
    setAddress(addr);
  };

  const buy = async () => {
    try {
      if (!signer) return alert("Connect wallet first");
      const inputToken = tokenAddresses[selectedNetwork][token];
      const erc20 = new Contract(inputToken, tokenABI, signer);
      const decimals: number = await erc20.decimals();
      const parsedAmount = ethers.parseUnits(amount, decimals);

      if (selectedNetwork !== targetNetwork) {
        const ethBalance = await signer.provider.getBalance(address);
        const requiredGas = ethers.parseEther("0.01");
        if (ethBalance < requiredGas) {
          setStatus("❌ Insufficient ETH balance for cross-chain gas fee (0.01 ETH required)");
          return;
        }
      }

      setStatus("⏳ Approving...");
      const approveTx = await erc20.approve(swapContractAddresses[selectedNetwork], parsedAmount);
      await approveTx.wait();

      const swapContract = new Contract(swapContractAddresses[selectedNetwork], swapABI, signer);

      if (selectedNetwork !== targetNetwork) {
        setStatus("🔄 Cross-chain swapping...");
        const dstEid = crossChainDestinations[targetNetwork as keyof typeof crossChainDestinations];
        const gasAmount = ethers.parseEther("0.01"); // 0.01 ETH для газа
        const swapTx = await swapContract.buyCrossChain(inputToken, parsedAmount, dstEid, address, { value: gasAmount });
        await swapTx.wait();
        setStatus(`✅ Cross-chain swap complete! TX: ${swapTx.hash}`);
      } else {
        setStatus("🔄 Swapping...");
        const swapTx = await swapContract.buy(inputToken, parsedAmount);
        await swapTx.wait();
        setStatus(`✅ Swap complete! TX: ${swapTx.hash}`);
      }

      fetchBalance();
    } catch (e: any) {
      console.error(e);
      setStatus("❌ Operation paused: chain is overloaded. Please, try again later.");
    }
  };

  const sell = async () => {
    try {
      if (!signer) return alert("Connect wallet first");
      const outputToken = tokenAddresses[selectedNetwork][token];
      const decimals = 6;
      const parsedGold = ethers.parseUnits(amount, decimals);
      const swapContract = new Contract(swapContractAddresses[selectedNetwork], swapABI, signer);
      setStatus("🔄 Selling...");
      const sellTx = await swapContract.sell(outputToken, parsedGold);
      await sellTx.wait();
      setStatus(`✅ Sale complete! TX: ${sellTx.hash}`);

      fetchBalance();
    } catch (e: any) {
      console.error(e);
      setStatus("❌ Operation paused: chain is overloaded. Please, try again.");
    }
  };

  const fetchGoldPrice = async (provider: BrowserProvider) => {
    try {
      const oracle = new Contract(oracleAddresses[selectedNetwork], oracleABI, provider);
      const rawPrice: bigint = await oracle.getPrice();
      const formatted = Number(rawPrice) / 1e18;
      setGoldPrice(formatted.toFixed(2));
    } catch (e: any) {
      setGoldPrice("Error");
    }
  };

  const switchNetwork = async (networkKey: string) => {
    if (!window.ethereum) return alert("Install MetaMask");

    const network = networks[networkKey as keyof typeof networks];
    if (!network) return alert("Unknown network");

    setIsLoadingData(true);
    try {
      await window.ethereum.request({
        method: 'wallet_switchEthereumChain',
        params: [{ chainId: network.chainId }],
      });
      setSelectedNetwork(networkKey);

      // Пересоздаем провайдер и синер для новой сети
      const newProvider = new ethers.BrowserProvider(window.ethereum);
      const newSigner = await newProvider.getSigner();
      setProvider(newProvider);
      setSigner(newSigner);

      // Обновляем данные после смены сети
      await fetchGoldPrice(newProvider);
      await fetchBalance();
    } catch (switchError: any) {
      // This error code indicates that the chain has not been added to MetaMask
      if (switchError.code === 4902) {
        try {
          await window.ethereum.request({
            method: "wallet_addEthereumChain",
            params: [network],
          });
          await window.ethereum.request({
            method: 'wallet_switchEthereumChain',
            params: [{ chainId: network.chainId }],
          });
          setSelectedNetwork(networkKey);

          // Пересоздаем провайдер и синер для новой сети
          const newProvider = new ethers.BrowserProvider(window.ethereum);
          const newSigner = await newProvider.getSigner();
          setProvider(newProvider);
          setSigner(newSigner);

          // Обновляем данные после добавления и смены сети
          await fetchGoldPrice(newProvider);
          await fetchBalance();
        } catch (e: any) {
          alert("Error adding network: " + (e.message || e));
        }
      } else {
        alert("Error switching network: " + (switchError.message || switchError));
      }
    } finally {
      setIsLoadingData(false);
    }
  };

  const getTokenMeta = (networkKey: string) => {
    const networkTokens = tokenAddresses[networkKey];
    if (!networkTokens) return {};

    return {
      USDC: {
        address: networkTokens.USDC,
        symbol: 'tUSD',
        decimals: 18,
        image: undefined,
      },
      GLD: {
        address: networkTokens.GLD,
        symbol: 'GLD',
        decimals: 6,
        image: window.location.origin + '/gold_logo.png',
      },
    };
  };

  const addSelectedTokenToMetaMask = async () => {
    if (!window.ethereum) return alert("Install MetaMask");
    const tokenMeta = getTokenMeta(selectedNetwork);
    const meta = tokenMeta[token as keyof typeof tokenMeta];
    if (!meta) return alert("Unknown token");
    try {
      await window.ethereum.request({
        method: 'wallet_watchAsset',
        params: {
          type: 'ERC20',
          options: {
            address: meta.address,
            symbol: meta.symbol,
            decimals: meta.decimals,
            image: meta.image,
          },
        },
      });
    } catch (e: any) {
      alert("Error adding token: " + (e.message || e));
    }
  };

  const addGoldTokenToMetaMask = async () => {
    if (!window.ethereum) return alert("Install MetaMask");
    const tokenMeta = getTokenMeta(selectedNetwork);
    const gldMeta = tokenMeta.GLD;
    if (!gldMeta) return alert("GLD token not found for selected network");
    try {
      await window.ethereum.request({
        method: 'wallet_watchAsset',
        params: {
          type: 'ERC20',
          options: {
            address: gldMeta.address,
            symbol: gldMeta.symbol,
            decimals: gldMeta.decimals,
            image: gldMeta.image,
          },
        },
      });
    } catch (e: any) {
      alert("Error adding token: " + (e.message || e));
    }
  };

  return (
    <div className="swap-container">
      <div className="swap-box">
        <h2 className="swap-title">
          <img src="/gold_logo.png" alt="MMM GOLD Logo" style={{ height: '2.2em', verticalAlign: 'middle', marginRight: '0.5em', borderRadius: '0.4em' }} />
          MMM GOLD Token Swap
        </h2>
        <Tabs tab={tab} setTab={setTab} />
        {!address ? (
          <>
            <button onClick={connectWallet} className="swap-button connect">
              🔗 Connect Wallet
            </button>
          </>
        ) : (
          <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
            <p className="swap-address">Connected: {address}</p>
          </div>
        )}
        <div className="swap-field">
          <label>Select network:</label>
          <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
            <select
              value={selectedNetwork}
              onChange={e => switchNetwork(e.target.value)}
              style={{ flex: 1, padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}
            >
              <option value="ethereumSepolia">Ethereum Sepolia</option>
              <option value="arbitrumSepolia">Arbitrum Sepolia</option>
            </select>
            <button
              onClick={() => switchNetwork(selectedNetwork)}
              disabled={isLoadingData}
              style={{
                padding: '8px 12px',
                borderRadius: '4px',
                border: '1px solid #ccc',
                background: isLoadingData ? '#e0e0e0' : '#f0f0f0',
                cursor: isLoadingData ? 'not-allowed' : 'pointer',
                fontSize: '0.9em',
                whiteSpace: 'nowrap',
                opacity: isLoadingData ? 0.7 : 1
              }}
              title={`Add network to MetaMask`}
            >
              {isLoadingData ? "🔄 Loading..." : "➕ Add"}
            </button>
          </div>
        </div>
        <div className="swap-field">
          <label>Select token:</label>
          <div style={{ display: 'flex', alignItems: 'center', gap: 6 }}>
            <select value={token} onChange={e => setToken(e.target.value)}>
              <option value="USDC">USDC</option>
            </select>
            {address && (
              <span
                onClick={addSelectedTokenToMetaMask}
                style={{ fontSize: '1em', color: '#888', cursor: 'pointer', textDecoration: 'none', outline: 'none', userSelect: 'none' }}
                title={`Add ${token} token to MetaMask`}
              >
                ➕
              </span>
            )}
          </div>
        </div>
        {tab === "buy" && (
          <div className="swap-field">
            <label>Target network:</label>
            <select
              value={targetNetwork}
              onChange={e => setTargetNetwork(e.target.value)}
              style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}
            >
              <option value="ethereumSepolia">Ethereum Sepolia</option>
              <option value="arbitrumSepolia">Arbitrum Sepolia</option>
            </select>
            {selectedNetwork !== targetNetwork && (
              <div style={{
                marginTop: '8px',
                padding: '8px',
                backgroundColor: '#fff3cd',
                border: '1px solid #ffeaa7',
                borderRadius: '4px',
                fontSize: '0.9em',
                color: '#856404'
              }}>
                🌉 Cross-chain transaction will be executed
                <br />
                💰 Additional 0.01 ETH gas fee required
              </div>
            )}
          </div>
        )}
        <p className="swap-price">
          💰 MMM GOLD Price: {isLoadingData ? "🔄 Loading..." : (goldPrice ? `$${goldPrice}` : "Loading...")}
        </p>
        <div className="swap-field">
          <label>Amount:</label>
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 4 }}>
            <span style={{ fontSize: '0.95em', color: '#bbb' }}>
              Balance: {isLoadingData ? "🔄 Loading..." : (balance ? balance : "-")}
            </span>
            <button
              type="button"
              style={{ fontSize: '0.9em', color: '#f0a500', background: 'none', border: 'none', cursor: 'pointer', padding: 0 }}
              onClick={() => setAmount(balance)}
              disabled={numericBalance <= 0}
            >
              Max
            </button>
          </div>
          <input
            type="number"
            value={amount}
            min={0}
            max={numericBalance}
            step={0.01}
            onChange={e => {
              let val = e.target.value;
              if (!/^\d*\.?\d*$/.test(val)) return;
              if (Number(val) > numericBalance) val = balance;
              setAmount(val);
            }}
            style={{ marginBottom: '0.5rem' }}
            disabled={numericBalance <= 0}
          />
          <input
            type="range"
            min={0}
            max={numericBalance}
            step={0.01}
            value={clampedAmount}
            onChange={e => setAmount(e.target.value)}
            className="amount-slider"
            disabled={numericBalance <= 0}
          />
        </div>
        {tab === "buy" ? (
          <>
            <button onClick={buy} className="swap-button">
              {selectedNetwork !== targetNetwork ? "🌉 Cross-Chain Buy MMM GOLD" : "💸 Buy MMM GOLD"}
            </button>
          </>
        ) : (
          <button onClick={sell} className="swap-button">
            💰 Sell MMM GOLD
          </button>
        )}
        {address && (
          <span
            onClick={addGoldTokenToMetaMask}
            style={{ display: 'block', marginTop: 8, fontSize: '0.95em', color: '#888', cursor: 'pointer', textDecoration: 'none', verticalAlign: 'middle', outline: 'none', userSelect: 'none' }}
            title={`Add MMM GOLD token to MetaMask`}
          >
            ➕ Add MMM GOLD token
          </span>
        )}
        <p className="swap-status">{status}</p>
      </div>
    </div>
  );
}

export default App;
