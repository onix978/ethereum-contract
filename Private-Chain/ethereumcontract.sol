pragma solidity ^0.4.22;

contract EthereumContract {
 
    string document;
    bool active;
    address requester;
    address recipient;

   function EthereumContract(address _requester, address _recipient, string _document) public {
        requester = _requester;
        recipient = _recipient;
        document = _document;
    }

    function getDocument() public constant returns (string) {
        return document;      
    }

    function getActive() public constant returns (bool) {
        return active;
    }

    function setActive(bool _active) public {
        active = _active;
    }  
}