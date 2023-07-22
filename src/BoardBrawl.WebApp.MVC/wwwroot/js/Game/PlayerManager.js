class PlayerManager {
    _gameHubConnection;

    constructor(gameHubConnection) {
        _gameHubConnection = gameHubConnection;
    }

    Load() {
        console.log('hello world');
    }
}