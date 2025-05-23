import { ProfileFormData } from "@/lib/schemas/profileSchema";

export interface ProfileProps {
    user: ProfileFormData,
    className?: string;
}