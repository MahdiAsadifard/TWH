import React, { useState, useEffect} from "react";
import { StatusCode } from "../../Global/RequestReponseHelper";
import { WebserviceUrl } from "../../Global/Config";

export type method = "GET" | "POST" | "UPDATE" | "DELETE";

interface IProps {
    url: string;
    method: method;
    body?: any;
};

const UseFetch = () => {
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState<boolean>(false);
    const [controller, setController] = useState<AbortController>();

    const fetchResponse = {
        statusCode: StatusCode.InternalServerError,
        success: false,
        response: null
    };

    const ApiRequest = async ({
        url,
        method,
        body,
    }: IProps) => {
         if(url.startsWith('/')) url = url.substring(0,1);
        url = `${WebserviceUrl}${url}`;
        const ctor = new AbortController();
        setController(ctor);

        setLoading(true)
        await fetch(url, {
            method,
            headers: {
            "Content-Type": 'application/json'
            },
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