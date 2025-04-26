import axios from 'axios';
import https from 'https';

// const httpsAgent = new https.Agent({
//   rejectUnauthorized: false, // Only for development with self-signed certs
// });

export interface Language {
  id: string; // Guids are returned as strings, even though they represent UUIDs
  name: string;
}

export interface LanguagesResponse {
  items: Language[]; // Array of Language objects
  count: number;      // Number of available languages
}

export async function getLanguages(token : string | null): Promise<LanguagesResponse> {
  try {
    const response = await axios.get<LanguagesResponse>(
      'https://localhost:7206/api/Owner/LKP_LanguageList',
      {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`, // 👈 pass the token
        },
        // httpsAgent
      }
    );

    return response.data;

  } catch (error: any) {
    throw new Error(error?.response?.data?.message || error.message);
  }
}
