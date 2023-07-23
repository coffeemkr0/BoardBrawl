class PlayerManager {

    _peerJsObject;
    _localStream;
    _peers;

    _streamStartedCallback;
    _streamEndedCallback;

    constructor(localStream, streamStarted, streamEnded) {
        this._localStream = localStream;
        this._peers = [];

        this._streamStartedCallback = streamStarted;
        this._streamEndedCallback = streamEnded;
    }

    AddPlayer(peerId) {
        this._peers.push(peerId);

        this.Call(peerId);
    }

    RemovePlayer(peerId) {
        const index = this._peers.indexOf(peerId);
        if (index !== -1) {
            this._peers.splice(index, 1);
        }
    }

    GetPeerJsObject() {
        return this._peerJsObject;
    }

    GetPeerId() {
        return this._peerJsObject.id;
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

    async Call(peerId, playerId) {
        var call = await new Promise((resolve, reject) => {
            const call = this._peerJsObject.call(peerId, _localVideoStream);

            this._peerJsObject.on('error', err => {
                if (err.message.includes(peerId)) {
                    this._peerJsObject.off('error');
                    reject(err.message);
                }
            });

            if (call) {
                call.on('stream', (stream) => {
                    this._peerJsObject.off('error');

                    const e = {
                        peerId: peerId,
                        playerId: playerId,
                        stream: stream
                    };
                    this._streamStartedCallback(e);

                    resolve(call);
                });
            }
            else {
                reject('Call did not go through');
            }
        });
    }
}