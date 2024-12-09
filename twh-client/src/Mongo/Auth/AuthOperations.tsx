import { WebserviceUrl } from '../../Global/Config';
import { StatusCode } from "../../Global/RequestReponseHelper";

export const CheckLogin = async(submission: any) => {
    const callback = {
        statusCode: StatusCode.InternalServerError,
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
        callback.statusCode = response.status;
        switch (response.status) {
            case StatusCode.OK:
            case StatusCode.NoContent:
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
    });
    return callback;
}; 