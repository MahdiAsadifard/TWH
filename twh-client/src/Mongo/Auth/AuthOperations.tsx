import { WebserviceUrl } from '../../Global/Config'

export const CheckLogin = async(submission: any) => {
    const callback = {
        statusCode: 500,
        success: false,
        response: null
    };
    const url = `${WebserviceUrl}auth/login`;
    await fetch(url, {
        method: 'POST',
        headers: {
           "Content-Type": 'application/json'
        },
        body: JSON.stringify(submission)
    })
    .then(response => {
        console.log("response status: ", response?.status)
        callback.statusCode = response.status;
        switch (response.status) {
            case 200:
            case 204:
                callback.success = true;
                return response.json();
                break;
        
            default:
                break;
        }
    })
    .then(responseJson => {
        if(callback.success) callback.response = responseJson;
    })
    .catch(error => {
        console.log(`Error on login`+ error);
    })
    .finally(()=>{
        return callback;
    });
}; 