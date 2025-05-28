'use client';

import { useAppSelector, useAppDispatch } from "@/lib/store/hooks";
import { EducationFormData, educationSchema } from "@/lib/schemas";
import { useEffect, useMemo } from "react";
import { institutionListQuery, degreeListQuery, fieldOfStudyListQuery, addEditEducation, educationListQuery } from "@/lib/apis/owner/education";
import { mapEducationToForm } from "@/lib/utils/appFunctions";
import { EducationProps } from "../types";
import { ControlledForm } from "@/components/ui/form";

const EducationForm = ({id, onClose} : EducationProps) => {

    const dispatch = useAppDispatch();
    const { loading, error, lstEducations, lstInstitutions, lstDegrees, lstFields } = useAppSelector((state) => state.education);
    const educationToHandle = lstEducations.find(ed => ed.id === id);

    const indicator = id ? {when: 'Update', while: 'Updating...'} : {when: 'Create', while: 'creating...'};

    const institutionOptions = useMemo(() =>
        lstInstitutions.map(i => ({ label: i.name, value: i.id }))
    , [lstInstitutions]);

    const degreeOptions = useMemo(() =>
        lstDegrees.map(i => ({ label: i.name, value: i.id }))
    , [lstDegrees]);

    const fieldOptions = useMemo(() =>
        lstFields.map(i => ({ label: i.name, value: i.id }))
    , [lstFields]);

    const onSubmit = async (data: EducationFormData) => {
        const resultAction = await dispatch(addEditEducation(data));

        if (!addEditEducation.rejected.match(resultAction)) {
            await dispatch(educationListQuery());
            onClose?.();
        }
    };

    useEffect(() => {
        lstInstitutions.length === 0 && dispatch(institutionListQuery());
        lstDegrees.length === 0 && dispatch(degreeListQuery());
        lstFields.length === 0 && dispatch(fieldOfStudyListQuery());
    }, []);

    return (
        <ControlledForm
            schema={educationSchema}
            onSubmit={onSubmit}
            items={[
                {as: 'Dropdown', name: 'LKP_InstitutionID', options: institutionOptions, label: 'Institution'},
                {as: 'Dropdown', name: 'LKP_DegreeID', options: degreeOptions, label: 'Degree'},
                {as: 'Dropdown', name: 'LKP_FieldOfStudyID', options: fieldOptions, label: 'Field of study'},
                {as: 'Input', name: 'startDate', label: 'Start date', type: 'Date'},
                {as: 'Input', name: 'endDate', label: 'End date', type: 'Date'},
                {as: 'Checkbox', name: 'isStudying', label: 'Still studying?'},
            ]}
            error={error}
            loading={loading}
            defaultValues={{isStudying: false}}
            watch={{
                name: 'isStudying',
                defaultValue: false,
                watched: 'endDate'
            }}
            resetItems={mapEducationToForm(educationToHandle) as any}
            indicator={indicator}
        />
    );
}

export default EducationForm;