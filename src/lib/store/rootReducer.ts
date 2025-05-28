import { combineReducers } from "redux";
import authSlice from "./slices/authSlice";
import searchSlice from "./slices/searchSlice";
import educationSlice from "./slices/educationSlice";
import ownerSlice from "./slices/ownerSlice";
import clientSlice from "./slices/clientSlice";
import experienceSlice from "./slices/experienceSlice";
import projectTechnologySlice from "./slices/projectTechnologySlice";
import userLanguageSlice from "./slices/userLanguageSlice";
import contactMessageSlice from './slices/contactMessage';

const rootReducer = combineReducers({
    auth: authSlice,
    search: searchSlice,
    owner: ownerSlice,
    client: clientSlice,
    education: educationSlice,
    experience: experienceSlice,
    projectTechnology: projectTechnologySlice,
    userLanguage: userLanguageSlice,
    contactMessage: contactMessageSlice
});

export default rootReducer;