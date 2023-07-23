class PlayerManager {

    _myPeer;
    _localStream;
    _remotePeers;

    _streamStartedCallback;
    _streamEndedCallback;

    constructor(localStream, streamStarted, streamEnded) {
        this._localStream = localStream;
        this._remotePeers = [];

        this._streamStartedCallback = streamStarted;
        this._streamEndedCallback = streamEnded;
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
                this._myPeer = new Peer(storedPeerJsId);
            } else {
                this._myPeer = new Peer();
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
            const peer = await GetRemotePeer();

            peer.on('error', err => {
                peer.off('error');
                reject(err.message);
            });

            const call = peer.call(peer.id, _localVideoStream);

            if (call) {
                call.on('close', () => {
                    OnCallDisrupted(peerId, playerId);
                });

                call.on('error', err => {
                    console.warn('Call error ' + err);

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
            const peer = new Peer();
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