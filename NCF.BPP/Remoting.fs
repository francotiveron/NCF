namespace NCF.BPP

open WebSharper

module Server =
    [<Rpc>]
    let getEmbedTokenAsync groupId reportId =
        let token = State.getEmbedToken groupId reportId
        async {return token}
