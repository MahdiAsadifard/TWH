import { FluentProvider, webLightTheme } from "@fluentui/react-components"
import { RouterProvider }  from "react-router"
import { router } from './Routs/Router';

import { UserProvider } from "./Loggedin/Common/UserProvider";
import "./Styles/App.css";

function App() {
  return (
    <FluentProvider theme={webLightTheme}>
      <UserProvider>
        <RouterProvider router={router} />
      </UserProvider>
    </FluentProvider>
  );
}

export default App;
