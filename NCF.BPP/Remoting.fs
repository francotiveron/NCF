namespace NCF.BPP

open WebSharper

module Server =
    [<Rpc>]
    let getEmbedTokenAsync groupId resourceId =
        let token = State.getEmbedToken groupId resourceId
        async {return token}
