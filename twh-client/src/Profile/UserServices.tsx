import * as UsersOperations from "../Mongo/Users/UsersOperations";

export const GetUsers = async (url?: string) => {
    const users = await UsersOperations.GetUsers(url);

    console.log("=== users: ", users);
};

