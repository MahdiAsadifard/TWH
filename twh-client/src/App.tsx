import { FluentProvider, webLightTheme } from "@fluentui/react-components"
import { RouterProvider }  from "react-router"
import { router } from './Routs/Router';

import "./Styles/App.css";

function App() {
  return (
    <FluentProvider theme={webLightTheme}>
      <RouterProvider router={router} />
    </FluentProvider>
  );
}

export default App;
