using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Assets;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using UnityEngine;
using UnityEngine.UI;

public class WalletNft : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update

    public Button newWallet;
    private int aprroved;
    private int mintedFeed;
    private int mintedHero;
    public string[] knights = new string[] {
        "Light_Knight", 
        "Blue_Knight", 
        "Brown_Knight", 
        "Yellow_Knight", 
        "Green_Knight", 
        "Grey_Knight", 
        "Red_Knight",
        "Pink_Knight", 
        "Purple_Knight",
        "Dark_Knight",
    };
    List<BigInteger> owner = new List<BigInteger>();
    List<string> OldKnights = new List<string>();

  
    private void createAccount()
    {
        var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
        var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
        var account = new Account(privateKey);
        GameUI.UpdateGuiText("WallletPublicKeyInput", account.Address);
        GameUI.UpdateGuiText("WallletPrivateKeyInput", account.PrivateKey);
        GameUI.ActiveButton("UpdateBalance", true);
    }

    private void importWallet()
    {
        var privateKey = GameObject.Find("InputPrivateKey").GetComponent<InputField>().text;
        if (privateKey.Length > 60)
        {
            var account = new Account(privateKey);
            GameUI.UpdateGuiText("WallletPublicKeyInput", account.Address);
            GameUI.UpdateGuiText("WallletPrivateKeyInput", account.PrivateKey);
        }

        GameUI.ActiveButton("UpdateBalance", true);
        GameUI.ActiveButton("ApprovalLink", true);
    }
    private async void getEthBalancer(string address = "0xf8331f18a7106bF6B9C0847b3BbC5B6180806A2C")
    {
        var web3 = ConnectWeb3.web3();
        var balance = await web3.Eth.GetBalance.SendRequestAsync(address);
        var etherAmount = Web3.Convert.FromWei(balance);
        GameUI.UpdateGuiText("MaticBalance", $"Matic: {etherAmount}");
    }

    private async void getLinkBalancer(string address = "0xf8331f18a7106bF6B9C0847b3BbC5B6180806A2C")
    {
        var web3 = ConnectWeb3.web3();
        var contract = web3.Eth.GetContract(LinkContract.Abi, LinkContract.Address);
        var balanceFunction = contract.GetFunction("balanceOf");
        var balance = await balanceFunction.CallAsync<BigInteger>(address);
        var etherAmount = Web3.Convert.FromWei(balance);
        GameUI.UpdateGuiText("LinkBalance", $"Link: {etherAmount}");
    }

    private async void LinkfeedBalancer(string address = "0xf8331f18a7106bF6B9C0847b3BbC5B6180806A2C")
    {
        var web3 = ConnectWeb3.web3();
        var contract = web3.Eth.GetContract(LinkHeroesContractcs.Abi, LinkHeroesContractcs.Address);
        var balanceFunction = contract.GetFunction("balanceOf");
        var balance = await balanceFunction.CallAsync<int>(address, 10);
        GameUI.UpdateGuiText("FeedLinkBalance", $"Linkfeed: {balance}");
        if (balance != 0)
        {
            GameUI.ActiveButton("Gethero", true);
        }
    }

    private async void HeroesBalancer(string address = "0xf8331f18a7106bF6B9C0847b3BbC5B6180806A2C")
    {
        var web3 = ConnectWeb3.web3();
        var contract = web3.Eth.GetContract(LinkHeroesContractcs.Abi, LinkHeroesContractcs.Address);
        var balanceFunction = contract.GetFunction("balanceOfBatch");
        var Batchaddress = new string[10];
        for (int i = 0; i < Batchaddress.Length; i++)
        {
            Batchaddress[i] = address;
        }
        var BatchHeroes = new BigInteger[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        owner = await balanceFunction.CallAsync<List<BigInteger>>(Batchaddress, BatchHeroes);

        
        for (int i = 0; i < OldKnights.Count; i++)
        {
            Destroy(GameObject.Find($"{OldKnights[i]}"));
        }

        for (int i = 0; i < knights.Length; i++)
        {
            if (owner[i] > 0)
            {
                for (int l = 0; l < int.Parse(owner[i].ToString()); l++)
                {
                    var randomX = Random.Range(0f, 10f);
                    var randomZ = Random.Range(-5f, 2f);
                    var knight = (GameObject)Resources.LoadAsync<GameObject>($"Knights/{knights[i]}").asset;
                    knight.transform.position = new UnityEngine.Vector3(randomX * 1.1f, 5, randomZ);
                    var prefab = Instantiate(knight);
                    OldKnights.Add($"{knights[i]}{l}");
                    prefab.name = $"{knights[i]}{l}";

                }
              
            }
        }
    }

    private async void ApproverLink()
    {
        var privateKey = GameObject.Find("WallletPrivateKeyInput").GetComponent<InputField>().text;
        var account = new Account(privateKey);
        var web3 = ConnectWeb3.web3(account);
        var contract = web3.Eth.GetContract(LinkContract.Abi, LinkContract.Address);
        var ApprolFunction = contract.GetFunction("approve");
        //var myParams = new object[] { "0x2C403402CBF2ea9D98d3EFDBB359e9e5fccF504E",  };
        var gas = new HexBigInteger(3000000);
        GameUI.ActiveButton("ApprovalLink", false);
        var receipt = await ApprolFunction.SendTransactionAndWaitForReceiptAsync(account.Address, gas, null, null, LinkHeroesContractcs.Address, new BigInteger(1000000000000000));
        aprroved = int.Parse(receipt.Status.ToString());
        GameUI.ActiveButton("ApprovalLink", true);
        GameUI.ActiveButton("MintLinkfeed", true);
        Debug.Log($"aprroved {aprroved}");
    }
    private async void mintlinkFeed()
    {
        var privateKey = GameObject.Find("WallletPrivateKeyInput").GetComponent<InputField>().text;
        Account account = new Account(privateKey);
        var web3 = ConnectWeb3.web3(account);
        var contract = web3.Eth.GetContract(LinkHeroesContractcs.Abi, LinkHeroesContractcs.Address);
        var ApprolFunction = contract.GetFunction("mintKey");
        //var myParams = new object[] { "0x2C403402CBF2ea9D98d3EFDBB359e9e5fccF504E",  };
        HexBigInteger gas = new HexBigInteger(3000000);
        GameUI.ActiveButton("MintLinkfeed", false);
        var receipt = await ApprolFunction.SendTransactionAndWaitForReceiptAsync(account.Address, gas, null, null, new BigInteger(1000000000000000));

        mintedFeed = int.Parse(receipt.Status.ToString());
        Debug.Log($"mintedFeed {mintedFeed}");
        LinkfeedBalancer(account.Address);
    }

    private async void MintHero()
    {
        var privateKey = GameObject.Find("WallletPrivateKeyInput").GetComponent<InputField>().text;
        var account = new Account(privateKey);
        var web3 = ConnectWeb3.web3(account);

        var contract = web3.Eth.GetContract(LinkHeroesContractcs.Abi, LinkHeroesContractcs.Address);
        var ApprolFunction = contract.GetFunction("getRandomNumber");
        var gas = new HexBigInteger(3000000);
        GameUI.ActiveButton("Gethero",false);
        var receipt = await ApprolFunction.SendTransactionAndWaitForReceiptAsync(account.Address, gas, null, null, new BigInteger(Random.Range(1, 10000)));
        GameUI.ActiveButton("Gethero", true);
        mintedHero = int.Parse(receipt.Status.ToString());
        Debug.Log($"mintedHero {mintedHero}");
    }
    void doExitGame()
    {
        Application.Quit();
    }
    private void Balance()
    {
        var address = GameObject.Find("WallletPublicKeyInput").GetComponent<InputField>().text;
        GameUI.ActiveButton("ApprovalLink", true);
        getEthBalancer(address);
        getLinkBalancer(address);
        LinkfeedBalancer(address);
        HeroesBalancer(address);
    }
  
    void Start()
    {

        GameObject.Find("NewWallet").
            GetComponent<Button>().
            onClick.AddListener(createAccount);

        GameObject.Find("ImportWallet").
            GetComponent<Button>().
            onClick.AddListener(importWallet);

        GameObject.Find("UpdateBalance").
            GetComponent<Button>().
            onClick.AddListener(Balance);

        GameObject.Find("ApprovalLink").
           GetComponent<Button>().
           onClick.AddListener(ApproverLink);

        GameObject.Find("MintLinkfeed").
          GetComponent<Button>().
          onClick.AddListener(mintlinkFeed);

        GameObject.Find("Gethero")
            .GetComponent<Button>()
            .onClick.AddListener(MintHero);

        GameObject.Find("QuitButton")
           .GetComponent<Button>()
           .onClick.AddListener(doExitGame);
        GameUI.UpdateGuiText("AddressNft", LinkHeroesContractcs.Address);
    }

    // Update is called once per frame
    void Update()
    {
        if (aprroved == 1)
        {
            
            aprroved = 0;
        }
        if (mintedFeed == 1)
        {
            Balance();
            mintedFeed = 0;
        }
    }
}
