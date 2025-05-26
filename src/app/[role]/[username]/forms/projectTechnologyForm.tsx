'use client';

import { useAppSelector, useAppDispatch } from "@/lib/store/hooks";
import { useEffect, useMemo } from "react";
import { ProjectTechnologyProps } from "../types";
import { ProjectTechnologyFormData, projectTechnologySchema } from "@/lib/schemas";
import { projectTechnologyListQuery, technologyListQuery, addEditDeleteProjectTechnology } from "@/lib/apis/owner/projectTechnology";
import { mapProjectTechnologyToForm } from "@/lib/utils/appFunctions";
import { ControlledForm } from "@/components/ui/form";

const ProjectTechnologyForm = ({id, onClose} : ProjectTechnologyProps) => {

    const dispatch = useAppDispatch();
    const { loading, error, lstProjectTechnologies, lstTechnologies } = useAppSelector((state) => state.projectTechnology);
    const projectTechnologyToHandle = lstProjectTechnologies.find(pt => pt.id === id);

    const indicator = id ? {when: 'Update', while: 'Updating...'} : {when: 'Create', while: 'creating...'};

    const technologyOptions = useMemo(() =>
        lstTechnologies.map(i => ({ label: i.name, value: i.id }))
    , [lstTechnologies]);

    const onSubmit = async (data: ProjectTechnologyFormData) => {
        await dispatch(addEditDeleteProjectTechnology(data));
        await dispatch(projectTechnologyListQuery());
        onClose?.();
    };

    useEffect(() => {
        lstTechnologies.length === 0 && dispatch(technologyListQuery());
    }, []);
 
    return (
        <ControlledForm
            schema={projectTechnologySchema}
            onSubmit={onSubmit}
            items={[
                {as: 'DropdownMulti', name: 'lstTechnologies', options: technologyOptions, label: 'Technologies'},
                {as: 'Input', name: 'title', label: 'Title', placeholder: 'Protfolio'},
                {as: 'Input', name: 'liveLink', label: 'Live link', placeholder: 'https://MyProject'},
                {as: 'Input', name: 'sourceCode', label: 'Source code', placeholder: 'https://LinkedIn'},
                {as: 'Input', name: 'imageUrl', label: 'Source code', placeholder: 'https://Image'},
                {as: 'Checkbox', name: 'isFeatured', label: 'Is featured?'},
                {as: 'Input', name: 'description', label: 'Description', placeholder: 'Description', type: 'Textarea'}
            ]}
            error={error}
            loading={loading}
            defaultValues={{isStudying: false}}
            resetItems={mapProjectTechnologyToForm(projectTechnologyToHandle) as any}
            indicator={indicator}
        />
    );
}

export default ProjectTechnologyForm;