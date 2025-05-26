import { createAsyncThunk } from "@reduxjs/toolkit";
import { dynamicApi } from "../../apiClient";

export const sortLanguage = createAsyncThunk(
    'userLanguage/sortLanguage',
    async (payload: string[], thunkAPI) => {
        try {
            const response = await dynamicApi({
                method: 'POST',
                url: '/Owner/SortLanguage',
                data: {languageIdsInOrder: payload},
                withCredentials: true
            });

            if (response.status === 400) return thunkAPI.rejectWithValue(response.data);

            return;

        } catch (error: any) {
            return thunkAPI.rejectWithValue(error.message);
        }
    }
);