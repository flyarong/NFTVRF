pragma solidity 0.6.6;

import 'https://github.com/OpenZeppelin/openzeppelin-contracts/blob/solc-0.6/contracts/token/ERC1155/ERC1155.sol';
import "https://github.com/smartcontractkit/chainlink/blob/develop/evm-contracts/src/v0.6/VRFConsumerBase.sol";

contract RandomNumberConsumer is VRFConsumerBase,ERC1155{
    
    bytes32 internal keyHash;
    uint256 internal fee;
    
    uint256 public randomResult;
    mapping(bytes32 => address) public requestToSender;

    enum Prizes
        {
        	Blue_Knight, 
        	Brown_Knight, 
        	Dark_Knight,
        	Yellow_Knight, 
        	Green_Knight, 
        	Grey_Knight, 
        	Light_Knight, 
        	Red_Knight,
        	Pink_Knight, 
        	Purple_Knight,
        	Chainlink_Key
        }
    
    constructor() 
        VRFConsumerBase(
            0xdD3782915140c8f3b190B5D67eAc6dc5760C46E9, // VRF Coordinator
            0xa36085F69e2889c224210F603D836748e7dC0088  // LINK Token
        )
        ERC1155("https://game.example/api/item/{id}.json")
        public
    {
        keyHash = 0x6c3699283bda56ad74f6b855546325b68d482e983852a7a82979cc4807b641f4;
        fee = 0.1 * 10 ** 18; // 0.1 LINK (varies by network)
    }

    event NewItem(address from,uint256 id, uint256 value);    
    
     function mintKey(uint256 amount) external{
        require(amount >= 1e18,"it requires more than 1 Link");
        LINK.transferFrom(msg.sender, address(this), amount);
        _mint(msg.sender, uint256(Prizes.Chainlink_Key), 10, "");
        emit NewItem(msg.sender,uint256(Prizes.Chainlink_Key),1);
     }
    
    function Balance() public view returns(uint256){
        return  LINK.balanceOf(address(this));
    }
    
    function getRandomNumber(uint256 userProvidedSeed) public returns (bytes32 requestId) {
        require(LINK.balanceOf(address(this)) >= fee, "Not enough LINK - fill contract with faucet");
        require(balanceOf(msg.sender,10) >= 1,"You don't have keys");
        _burn(msg.sender, uint256(Prizes.Chainlink_Key), 1);
        bytes32 requestId = requestRandomness(keyHash, fee, userProvidedSeed);
        requestToSender[requestId] = msg.sender;
        return requestId;
    }

    /**
     * Callback function used by VRF Coordinator
     */
    function fulfillRandomness(bytes32 requestId, uint256 randomness) internal override {
        randomResult = randomness;
        _mint(requestToSender[requestId], randomResult % 10, 1, "");
    }
}