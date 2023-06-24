import { lazy } from "react";

const TestBuilder = lazy(() => import("../components/testbuilder/TestBuilder"));

const tests = [
  {
    path: "/test/:testId/builder",
    name: "Test Builder",
    exact: true,
    element: TestBuilder,
    roles: ["Admin"],
    isAnonymous: false,
  }
];

const allRoutes = [
  ...tests
];

export default allRoutes;
