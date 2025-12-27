import { useState, useEffect } from "react";
import { useNavigate } from "react-router";
import { StatusCode } from "../../Global/RequestReponseHelper";
import { paths } from "../../Routs/Router";
import { WebserviceUrl } from "../../Global/Config";
import * as Utils from "../../Global/Utils";
import { ILoginReponse } from "../../Login/Types";

export type method = "GET" | "POST" | "UPDATE" | "DELETE";

interface IProps {
    url: string;
    method: method;
    body?: any;
    withToken?: boolean;
}

interface ICallApiProps {
    url: string;
    method: method;
    body?: any;
    withToken?: boolean;
    abortController: AbortController;
}

export interface IFetchResponse {
    statusCode: StatusCode;
    success: boolean;
    response: any;
}

let fetchResponse: IFetchResponse = {
    statusCode: StatusCode.InternalServerError,
    success: false,
    response: null
}

const UseFetch = () => {
    const navigate = useNavigate();
    const [abortController, setAbortController] = useState<AbortController>(new AbortController()); 
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<boolean>(false);

    const { customerUri } = Utils.GetCookies();

    const ApiRequest = async (props: IProps) => {
        try {
            setLoading(true);
            setError(false);

            const initial = await callFetch({...props, abortController});
            fetchResponse=initial;
            
            if (initial.statusCode === StatusCode.Unauthorized) {
                const refreshUrl = `${customerUri}/auth/resettokens`;
                
                const reset = await callFetch({
                    url: refreshUrl,
                    method: "POST",
                    withToken: false,
                    abortController
                });
                Utils.SetCookies(reset.response as ILoginReponse);
                fetchResponse = reset;

                if (reset.success) {
                    const retry = await callFetch({...props, abortController});
                    fetchResponse = retry;
                } else {
                    if (reset.statusCode === StatusCode.Unauthorized) {
                        Utils.DeleteAllCookies();
                        navigate(paths.slash);
                    }
                }
            }
            setLoading(false);

        } catch (error) {
            setError(true);
            setLoading(false);
            console.error("ApiRequest error:", error);           
        }
    };

    useEffect(() => {
        return () => abortController && abortController?.abort();
    }, [abortController]);

    return { loading, error, fetchResponse, ApiRequest };
};

const callFetch = async ({
        url,
        method,
        body,
        withToken = true,
        abortController
    }: ICallApiProps)
    : Promise<IFetchResponse> => {
    const { token, refreshToken } = Utils.GetCookies();

    if (url.startsWith("/")) url = url.substring(0, 1);
    url = `${WebserviceUrl}${url}`;
    const headers: HeadersInit = {
        "Content-Type": "application/json",
        ...(withToken && {
            Authorization: `Bearer ${token}`,
            "X-Refresh-Token": refreshToken
        })
    };

    try {
        const response = await fetch(url, {
            method,
            headers,
            body: body ? JSON.stringify(body) : undefined,
           // signal: abortController.signal // TODO: fix abort controller >>> existing with no reason issue 
        });

        fetchResponse.statusCode = response.status;

        if (response.status === StatusCode.OK || response.status === StatusCode.NoContent) {
            fetchResponse.success = true;
            fetchResponse.response = await response.json();
        }
    } catch (err) {
        console.error("Fetch error:", err);
    }
    return fetchResponse;
};

export default UseFetch;