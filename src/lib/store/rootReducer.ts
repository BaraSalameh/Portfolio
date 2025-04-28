import { combineReducers } from "redux";
import authSlice from "./slices/authSlice";
import searchSlice from "./slices/searchSlice";
import userSlice from "./slices/userSlice";
import educationSlice from "./slices/educationSlice";


const rootReducer = combineReducers({
    auth: authSlice,
    search: searchSlice,
    user: userSlice,
    education: educationSlice
});

export default rootReducer;