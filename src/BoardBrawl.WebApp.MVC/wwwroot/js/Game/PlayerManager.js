class PlayerManager {

    _myPeer;
    _localStream;
    _remotePeers;
    _peerConfigOptions;

    _streamStartedCallback;
    _streamEndedCallback;

    constructor(localStream, streamStarted, streamEnded) {
        this._localStream = localStream;
        this._remotePeers = [];

        this._streamStartedCallback = streamStarted;
        this._streamEndedCallback = streamEnded;

        this._peerConfigOptions = {
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
    }

    AddPlayer(peerId) {
        this._peers.push(peerId);

        this.Call(peerId);
    }

    RemovePlayer(peerId, playerId) {
        const index = this._peers.indexOf(peerId);
        if (index !== -1) {
            this._peers.splice(index, 1);
        }

        //TODO:close call, dispose stuff etc.

        this.OnCallDisrupted(peerId, playerId);
    }

    GetPeerId() {
        return this._myPeer.id;
    }

    async InitializePeerJs() {
        await new Promise((resolve, reject) => {
            const storedPeerJsId = sessionStorage.getItem('peerJsId');
            if (storedPeerJsId) {
                this._myPeer = new Peer(storedPeerJsId, this._peerConfigOptions);
            } else {
                this._myPeer = new Peer(this._peerConfigOptions);
            }

            if (!this._myPeer) reject('Peer Js object did not initialize.');

            this._myPeer.on('open', id => {
                sessionStorage.setItem('peerJsId', id);
                resolve();
            });

            this._myPeer.on("call", (call) => {
                call.answer(this._localStream);
            });
        });
    }

    async Call(peerId, playerId) {
        var call = await new Promise(async (resolve, reject) => {
            const peer = await this.GetRemotePeer();

            peer.on('error', err => {
                console.log('Peer error with playerId ' + playerId + ' ' + err);
                reject(err.message);
            });

            const myStream = await navigator.mediaDevices.getUserMedia({
                audio: true,
                video: true
            });

            const call = peer.call(peerId, myStream);

            if (call) {
                call.on('close', () => {
                    console.warn('Call closed to playerId ' + playerId);

                    OnCallDisrupted(peerId, playerId);
                });

                call.on('error', err => {
                    console.warn('Call error with playerId ' + playerId + ' ' + err);

                    OnCallDisrupted(peerId, playerId);
                });

                call.on('stream', (stream) => {
                    peer.off('error');

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

    async GetRemotePeer() {
        const remotePeer = await new Promise((resolve, reject) => {
            const peer = new Peer(this._peerConfigOptions);
            if (!peer) reject('Peer Js object did not initialize.');

            peer.on('open', id => {
                resolve(peer);
            });

            //TODO:Setup error handling for the peer (should be better now that there is a dedicated peer for the other player)
        });

        return remotePeer;
    }

    OnCallDisrupted(peerId, playerId) {
        const e = {
            peerId: peerId,
            playerId: playerId
        };

        OnPlayerStreamEnded(e);
    }
}