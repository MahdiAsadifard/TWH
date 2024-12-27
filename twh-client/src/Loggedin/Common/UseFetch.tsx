import React, { useState, useEffect} from "react";
import { StatusCode } from "../../Global/RequestReponseHelper";
import { WebserviceUrl } from "../../Global/Config";
import * as Utils from "../../Global/Utils";

export type method = "GET" | "POST" | "UPDATE" | "DELETE";

interface IProps {
    url: string;
    method: method;
    body?: any;
    withToken?: boolean;
};

const UseFetch = () => {
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [controller, setController] = useState<AbortController>();
    const token = Utils.GetTokenFromCookies();

    const fetchResponse = {
        statusCode: StatusCode.InternalServerError,
        success: false,
        response: null
    };

    const ApiRequest = async ({
        url,
        method,
        body,
        withToken = true,
    }: IProps) => {
         if(url.startsWith('/')) url = url.substring(0,1);
        url = `${WebserviceUrl}${url}`;
        const ctor = new AbortController();
        setController(ctor);

        const headers: HeadersInit = {
            "Content-Type": 'application/json',
            ...(withToken &&  {"Authorization": `Bearer ${token} `})
        }
        
        setLoading(true)
        await fetch(url, {
            method,
            headers,
            signal: ctor.signal,
            body: JSON.stringify(body)
        })
        .then(response => {
            fetchResponse.statusCode = response.status;
            switch (response.status) {
                case StatusCode.OK:
                case StatusCode.NoContent:
                    fetchResponse.success = true;
                    return response.json();
            
                default:
                    break;
            }
        })
        .then(responseJson => {
            if(fetchResponse.success) fetchResponse.response = responseJson;
        })
        .catch(error => {
            console.log(`Error on login`+ error);
            setError(error);
        })
        .finally(() =>{
            setLoading(false);
        });
    };

    useEffect(() => {
        return () => controller && controller?.abort();
    }, [controller]);

    return {error, loading, fetchResponse, ApiRequest};
};

export default UseFetch;