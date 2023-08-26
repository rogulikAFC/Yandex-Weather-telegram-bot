import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import "./reset.css";
import "./Form/Form.css"
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import RegistrationPage from './pages/RegistrationPage'

const router = createBrowserRouter([
  {
    path: "registration/:userTelegramId",
    element: <RegistrationPage></RegistrationPage>,
  },
]);

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
