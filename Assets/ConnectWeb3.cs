using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class ConnectWeb3
    {
        public static Web3 web3(Account account) => new Web3(account, "https://rpc-mumbai.maticvigil.com/");
        public static Web3 web3() => new Web3("https://rpc-mumbai.maticvigil.com/");
    }
}
