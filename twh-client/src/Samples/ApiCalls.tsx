import useFetch from "../Loggedin/Common/UseFetch";

const ApiCalls = () => {

    const {  error, loading, fetchResponse, ApiRequest } =  useFetch();

    const click = async() => {
        const url = `d1f2f/user`;
        await ApiRequest({url, method: 'GET', body: null, withToken: true});
        console.log ("== response: ",fetchResponse);

    }

    return (
        <div>
            <button type="button" 
             title="Click"
             onClick={() => click()}
            style={{
                backgroundColor:'blue',
                width: 100,
                height: 100,
                color: 'white',
            }}  />

        </div>
    );
};

export default ApiCalls;