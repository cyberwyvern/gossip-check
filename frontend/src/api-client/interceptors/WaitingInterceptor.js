export default function WaitingInterceptor(apiClient, rootStore) {
  return function () {
    const { loadingProgressStore } = rootStore;
    const { incrementPendingRequestsNumber, decrementPendingRequestsNumber } = loadingProgressStore;
    const client = apiClient.apply(this, arguments);

    client.interceptors.request.use((config) => {
      incrementPendingRequestsNumber();
      return config;
    });

    client.interceptors.response.use((response) => {
      decrementPendingRequestsNumber();
      return response;
    }, (error) => {
      decrementPendingRequestsNumber();
      return Promise.reject(error);
    });

    return client;
  }
}
