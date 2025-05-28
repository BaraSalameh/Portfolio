'use client';

import { useAppSelector, useAppDispatch } from "@/lib/store/hooks";
import { ContactMessageFormData, contactMessageSchema } from "@/lib/schemas";
import { EducationProps } from "../types";
import { ControlledForm } from "@/components/ui/form";
import { sendEmail } from "@/lib/apis";

const ContactMessageForm = ({onClose} : EducationProps) => {

    const dispatch = useAppDispatch();
    const { loading, error, user } = useAppSelector((state) => state.client);

    const messagePlaceholder = `Dear ${user?.firstname} ${user?.lastname}...`;

    const onSubmit = async (data: ContactMessageFormData) => {
        const resultAction = await dispatch(sendEmail(data));

        if (!sendEmail.rejected.match(resultAction)) {
            onClose?.();
        }
    };

    return (
        <ControlledForm
            schema={contactMessageSchema}
            onSubmit={onSubmit}
            items={[
                {as: 'Input', name: 'emailTo', label: 'To', placeholder: 'john.doe@example.com', type: 'Email', config: ['Disabled']},
                {as: 'Input', name: 'name', label: 'Full name', placeholder: 'John Doe'},
                {as: 'Input', name: 'email', label: 'Email', placeholder: 'john.doe@example.com', type: 'Email'},
                {as: 'Input', name: 'subject', label: 'Subject', placeholder: 'Job oppurtunity'},
                {as: 'Input', name: 'message', label: 'Body', placeholder: messagePlaceholder, type: 'Textarea'}
            ]}
            error={error}
            loading={loading}
            defaultValues={{emailTo: user?.email ?? '', message: `${messagePlaceholder}\n\n`}}
            indicator={{when: 'Send', while: 'Sending...'}}
        />
    );
}

export default ContactMessageForm;