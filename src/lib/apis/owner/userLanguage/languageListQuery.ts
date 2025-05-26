import { createAsyncThunk } from '@reduxjs/toolkit';
import { dynamicApi } from '../../apiClient';

export const languageListQuery = createAsyncThunk(
    'userLanguage/languageListQuery',
    async (_, thunkAPI)  => {
        try {

            const response = await dynamicApi({
                method: 'GET',
                url: '/Owner/LKP_LanguageList',
                withCredentials: true
            });

            if (response.status === 204) return [];

            return [...response.data.items];

        } catch (error: any) {
            return thunkAPI.rejectWithValue(error.message);
        }
    }
);
