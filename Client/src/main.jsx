import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import "./reset.css";
import "./Form/Form.css";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import RegistrationPage from "./pages/RegistrationPage/RegistrationPage";
import PlacesPage from "./pages/PlacesPage/PlacesPage";

const router = createBrowserRouter([
  {
    path: ":userId",
    element: <PlacesPage />,
  },
  {
    path: "registration/:userTelegramId",
    element: <RegistrationPage />,
  },
]);

ReactDOM.createRoot(document.getElementById("root")).render(
  // <React.StrictMode>
    <RouterProvider router={router} />
  // </React.StrictMode>
);
