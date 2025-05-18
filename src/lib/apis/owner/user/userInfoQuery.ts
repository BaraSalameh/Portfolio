import { createAsyncThunk } from '@reduxjs/toolkit';
import { dynamicApi } from '../../apiClient';

export const userInfoQuery = createAsyncThunk(
    'owner/userInfoQuery',
    async (_, thunkAPI)  => {
        try {
            const response = await dynamicApi({
                method: 'GET',
                url: '/Owner/UserInfo',
                withCredentials: true
            });

            if (response.status === 400) {
                return thunkAPI.rejectWithValue(response.data);
            }

            return response.data;

        } catch (error: any) {
            return thunkAPI.rejectWithValue(error.message);
        }
    }
);
