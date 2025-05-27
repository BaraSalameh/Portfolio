import { z } from 'zod';

const guidRegex = /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[1-5][0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12}$/;

export const userLanguageSchema = z.object({
    lstLanguages: z.array(
        z.object({
            lkP_LanguageID: z
                .string()
                .min(1, 'Language is required')
                .regex(guidRegex, 'Language ID must be a valid GUID'),
            lkP_LanguageProficiencyID: z
                .string()
                .min(1, 'Proficiency is required')
                .regex(guidRegex, 'Proficiency ID must be a valid GUID'),
        })
  ),
});

export type UserLanguageFormData = z.infer<typeof userLanguageSchema>;