import * as UserServices from "./UserServices";


const ProfileView = () => {
    const click = async(url?: string) => {
                console.log("clicked")
                await UserServices.GetUsers(url);
             }
    return (
        <div>
            <button type="button" 
             title="Click"
             onClick={()=>click()}
            style={{
                backgroundColor:'red',
                width: 100,
                height: 100,
                color: 'white',
            }}  />
            <button type="button" 
             title="Click"
             onClick={() => click ("http://localhost:5006/api/user")}
            style={{
                backgroundColor:'blue',
                width: 100,
                height: 100,
                color: 'white',
            }}  />

        </div>
    );
};

export default ProfileView;