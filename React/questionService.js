import axios from "axios";
import {
  onGlobalError,
  onGlobalSuccess,
  API_HOST_PREFIX,
} from "./serviceHelpers";

const questionService = {
  endpoint: `${API_HOST_PREFIX}/api/questions`,
};

const addQuestion = (payload) => {
  const config = {
    method: "POST",
    url: questionService.endpoint,
    data: payload,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const getById = (payload) => {
  const config = {
    method: "GET",
    url: questionService.endpoint,
    data: payload,
    headers: { "Content-Type": "application/json" }
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

export { addQuestion, getById };