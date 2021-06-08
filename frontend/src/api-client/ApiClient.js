import axios from "axios";

export default function ApiClient(baseUrl) {
  const api = axios.create({
    baseURL: baseUrl,
    headers: {
      'Content-type': 'application/json; charset=utf-8',
      'Accept': 'application/json; charset=utf-8'
    }
  });

  return api;
}