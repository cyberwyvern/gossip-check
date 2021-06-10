export default function ErrorInerceptor(apiClient, { alertStore: { showError } }) {
  return function () {
    const client = apiClient.apply(this, arguments);
    client.interceptors.response.use(null, (error) => {
      showError("error");
      return Promise.reject(error);
    });

    return client;
  }
}
