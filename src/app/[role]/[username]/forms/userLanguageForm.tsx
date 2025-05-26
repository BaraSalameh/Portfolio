'use client';

import { useAppSelector, useAppDispatch } from "@/lib/store/hooks";
import { useEffect, useMemo } from "react";
import { UserLanguageProps } from "../types";
import { ControlledForm } from "@/components/ui/form";
import { UserLanguageFormData, userLanguageSchema } from "@/lib/schemas";
import { userLanguageListQuery, languageListQuery, languageProficiencyListQuery, editDeleteUserLanguage } from "@/lib/apis";
import { mapUserLanguageToForm } from "@/lib/utils/appFunctions";

const UserLanguageForm = ({id, onClose} : UserLanguageProps) => {

    const dispatch = useAppDispatch();
    const { loading, error, lstUserLanguages, lstLanguages, lstLanguageProficiencies } = useAppSelector((state) => state.userLanguage);
    const userLanguageToHandle = lstUserLanguages.find(ul => ul.lkP_LanguageID === id);

    const languageOptions = useMemo(() =>
        lstLanguages.map(i => ({ label: i.name, value: i.id }))
    , [lstLanguages]);

    const languageProficiencyOptions = useMemo(() =>
        lstLanguageProficiencies.map(i => ({ label: i.level, value: i.id }))
    , [lstLanguageProficiencies]);

    const onSubmit = async (data: UserLanguageFormData) => {
        await dispatch(editDeleteUserLanguage({...lstUserLanguages, ...data}));
        await dispatch(userLanguageListQuery());
        onClose?.();
    };

    useEffect(() => {
        lstLanguages.length === 0 && dispatch(languageListQuery());
        lstLanguageProficiencies.length === 0 && dispatch(languageProficiencyListQuery());
    }, []);

    return (
        <ControlledForm
            schema={userLanguageSchema}
            onSubmit={onSubmit}
            items={[
                {as: 'Dropdown', name: 'lkP_LanguageID', options: languageOptions, label: 'Language'},
                {as: 'Dropdown', name: 'lkP_LanguageProficiencyID', options: languageProficiencyOptions, label: 'Proficiency'}
            ]}
            error={error}
            loading={loading}
            resetItems={mapUserLanguageToForm(userLanguageToHandle) as any}
            indicator={{when: 'Update', while: 'Updating...'}}
        />
    );
}

export default UserLanguageForm;