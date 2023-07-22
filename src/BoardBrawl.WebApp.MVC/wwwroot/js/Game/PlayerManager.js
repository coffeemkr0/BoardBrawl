class PlayerManager {
    _gameHubConnection;

    constructor(gameHubConnection) {
        this._gameHubConnection = gameHubConnection;

        this._gameHubConnection.on("OnPlayerJoined", this.OnPlayerJoined);
        this._gameHubConnection.on("OnPlayerDisconnected", this.OnPlayerDisconnected);
    }

    Load() {
        console.log(`Player Manager Load`);
    }

    //Event handlers
    OnPlayerJoined(playerId, peerId) {
        console.log(`Player Manager OnPlayerJoined: playerId:${playerId}, peerId:${peerId}`);
    }

    OnPlayerDisconnected(playerId) {
        console.log(`Player Manager OnPlayerDisconnected: playerId:${playerId}`);
    }
}