
export const GetUsers = async (url?:string) => {
    const callback = {
        success: false,
        response: null,
        status: 0
    };
    // const url = `http://localhost:5005/api/user`;
    url = url ?? `http://localhost:5005/api/user`;
    console.log(url)
    await fetch(url, {
        method: 'GET',
        headers: {
            "Content-Type": "application/json",
            //"Access-Control-Allow-Origin": "*"
        }
    })
    .then(response => {
        callback.status = response.status;
        console.log(response.status, callback.status);
        console.log(response);
        switch (response.status) {
            case 200:
                console.log('OKAY');
                callback.success = true;
                return response.json();
        
            default:
                break;
        };
    })
    .then((responseJson) => {
        console.log("responseJson: ",responseJson);
        if(callback.success) callback.response = responseJson;
    })
    .catch(er => {
        console.log(`ERROR: ${er}`);
    });
    return callback;    
};