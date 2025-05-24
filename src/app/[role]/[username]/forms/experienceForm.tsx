'use client';

import { useAppSelector, useAppDispatch } from "@/lib/store/hooks";
import { ExperienceFormData, experienceSchema } from "@/lib/schemas";
import { addEditExperience, experienceListQuery } from "@/lib/apis/owner/experience";
import { ExperienceProps } from "../types";
import { ControlledForm } from "@/components/ui/form";

const ExperienceForm = ({id, onClose} : ExperienceProps) => {

    const dispatch = useAppDispatch();
    const { loading, error, lstExperiences } = useAppSelector((state) => state.experience);
    const experienceToHandle = lstExperiences.find(ex => ex.id === id);
    const indicator = id ? {when: 'Update', while: 'Updating...'} : {when: 'Create', while: 'creating...'};

    const onSubmit = async (data: ExperienceFormData) => {
        await dispatch(addEditExperience(data));
        await dispatch(experienceListQuery());
        onClose?.();
    };

    return (
        <ControlledForm
            schema={experienceSchema}
            onSubmit={onSubmit}
            items={[
                {as: 'Input', name: 'companyName', label: 'Company', placeholder: 'Google'},
                {as: 'Input', name: 'jobTitle', label: 'Job title', placeholder: 'Software Developer'},
                {as: 'Input', name: 'startDate', label: 'Start date', type: 'Date'},
                {as: 'Input', name: 'endDate', label: 'End date', type: 'Date'},
                {as: 'Checkbox', name: 'isWorking', label: 'Still working?'},
                {as: 'Input', name: 'location', label: 'Location', placeholder: 'Champs-Élysées St - Paris'},
                {as: 'Input', name: 'description', label: 'Description', placeholder: 'Description', type: 'Textarea'}
            ]}
            error={error}
            loading={loading}
            defaultValues={{isStudying: false}}
            watch={{
                name: 'isWorking',
                defaultValue: false,
                watched: 'endDate'
            }}
            resetItems={experienceToHandle as any}
            indicator={indicator}
        />
    );
}

export default ExperienceForm;