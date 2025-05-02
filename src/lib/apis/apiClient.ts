import axios, { AxiosRequestConfig, AxiosResponse } from 'axios';

const api = axios.create({
    baseURL: 'https://localhost:7206/api',
    withCredentials: true, 
});

let isRefreshing = false;

const refreshAccessToken = async () => {
    if (!isRefreshing) {
        isRefreshing = true;
        try {
            await api.post('/Account/ValidateToken'); 
        } finally {
            isRefreshing = false;
        }
    }
};

type HTTPMethod = 'GET' | 'POST' | 'PUT' | 'PATCH' | 'DELETE';

interface DynamicApiOptions extends AxiosRequestConfig {
    method: HTTPMethod;
    url: string;
    data?: any;
}

export const dynamicApi = async <T = any>(
    options: DynamicApiOptions
): Promise<AxiosResponse<T>> => {
    try {
        return await api.request<T>(options);
    } catch (error: any) {
        // If unauthorized and not already retried, try refresh
        if (error.response?.status === 401 && !options.headers?.['x-retry']) {
            try {
                await refreshAccessToken();
                return await api.request<T>({
                    ...options,
                    headers: {
                        ...(options.headers || {}),
                        'x-retry': 'true', // Prevent infinite loop
                    },
                });
            } catch (refreshError) {
                // Optionally redirect to login if refresh fails
                window.location.href = '/login';
            }
        }
        throw error;
    }
};
