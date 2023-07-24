class PlayerManagerMultiPeer {

    _peerJsObject;
    _localStream;
    _peerIds;
    _peers;

    _streamStartedCallback;
    _streamEndedCallback;

    constructor(localStream, streamStarted, streamEnded) {
        this._localStream = localStream;
        this._peerIds = [];

        this._streamStartedCallback = streamStarted;
        this._streamEndedCallback = streamEnded;
    }

    AddPlayer(peerId) {
        this._peerIds.push(peerId);

        this.Call(peerId);
    }

    RemovePlayer(peerId, playerId) {
        const index = this._peerIds.indexOf(peerId);
        if (index !== -1) {
            this._peerIds.splice(index, 1);
        }

        //TODO:close call, dispose stuff etc.

        this.OnCallDisrupted(peerId, playerId);
    }

    GetPeerId() {
        return this._peerJsObject.id;
    }

    async InitializePeerJs() {
        await new Promise((resolve, reject) => {
            const options = {
                config: {
                    "iceServers": [
                        {
                            "urls": "stun:stun.relay.metered.ca:80"
                        },
                        {
                            "credential": "JrO10hPpURmM0epS",
                            "urls": "turn:a.relay.metered.ca:80",
                            "username": "27f97c611a2020a66ea4d282"
                        },
                        {
                            "credential": "JrO10hPpURmM0epS",
                            "urls": "turn:a.relay.metered.ca:80?transport=tcp",
                            "username": "27f97c611a2020a66ea4d282"
                        },
                        {
                            "credential": "JrO10hPpURmM0epS",
                            "urls": "turn:a.relay.metered.ca:443",
                            "username": "27f97c611a2020a66ea4d282"
                        },
                        {
                            "credential": "JrO10hPpURmM0epS",
                            "urls": "turn:a.relay.metered.ca:443?transport=tcp",
                            "username": "27f97c611a2020a66ea4d282"
                        }
                    ]
                }
            };
            const storedPeerJsId = sessionStorage.getItem('peerJsId');

            if (storedPeerJsId) {
                this._peerJsObject = new Peer(storedPeerJsId, options);
            } else {
                this._peerJsObject = new Peer(options);
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
                call.on('close', () => {
                    OnCallDisrupted(peerId, playerId);
                });

                call.on('error', err => {
                    console.warn('Call error ' + err);

                    OnCallDisrupted(peerId, playerId);
                });

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

    OnCallDisrupted(peerId, playerId) {
        const e = {
            peerId: peerId,
            playerId: playerId
        };

        OnPlayerStreamEnded(e);
    }
}