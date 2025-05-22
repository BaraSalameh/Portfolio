import { z } from 'zod';

export const projectTechnologySchema = z.object({
    id: z.string().optional(),

    title: z
        .string()
        .min(3, 'Title is too short'),

    liveLink: z
        .string()
        .min(3, 'Live link is too short'),
        
    sourceCode: z
        .string()
        .min(3, 'Source code is too short'),

    imageUrl: z
        .string()
        .max(1000, 'Image string is too long'),

    description: z.string()
        .max(1000, 'Description is too long'),

    isFeatured: z
        .boolean()
        .optional(),
    
    lstTechnologies: z
        .array(z.string())
});

export const technologySchema = z.object({
    id: z.string(),
    name: z
        .string()
        .min(3, 'Name is too short'),
    iconUrl: z
        .string()
        .max(1000, 'Image string is too long'),
});

export type ProjectTechnologyFormData = z.infer<typeof projectTechnologySchema>;
export type TechnologyFormData = z.infer<typeof technologySchema>;