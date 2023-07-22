class PlayerManager {
    _gameHubConnection;
    _peerJsObject;
    _localStream;

    constructor(gameHubConnection, localStream) {
        this._gameHubConnection = gameHubConnection;
        this._localStream = localStream;

        this._gameHubConnection.on("OnPlayerJoined", this.OnPlayerJoined);
        this._gameHubConnection.on("OnPlayerDisconnected", this.OnPlayerDisconnected);
    }

    //Event handlers
    OnPlayerJoined(playerId, peerId) {
        console.log(`Player Manager OnPlayerJoined: playerId:${playerId}, peerId:${peerId}`);
    }

    OnPlayerDisconnected(playerId) {
        console.log(`Player Manager OnPlayerDisconnected: playerId:${playerId}`);
    }

    GetPeerJsObject() {
        return this._peerJsObject;
    }

    async InitializePeerJs() {
        await new Promise((resolve, reject) => {
            const storedPeerJsId = sessionStorage.getItem('peerJsId');
            if (storedPeerJsId) {
                this._peerJsObject = new Peer(storedPeerJsId);
            } else {
                this._peerJsObject = new Peer();
            }

            if (!this._peerJsObject) reject('Peer Js object did not initialize.');

            this._peerJsObject.on('open', id => {
                sessionStorage.setItem('peerJsId', id);
                resolve();
            });

            this._peerJsObject.on("call", (call) => {
                call.answer(this._localStream);
            });
        });
    }
}