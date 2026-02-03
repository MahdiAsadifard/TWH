import React, { useEffect } from "react";

type FileType = 'image' | 'video' | 'audio' | 'document' | 'other';

interface IProps {
    fileType: FileType;
    customStyle?: string;
    onClickCallback?: (file: any) => void;
    children?: React.ReactNode;
}

const CustomUpload: React.FunctionComponent<IProps>  = ({
    fileType,
    customStyle,
    onClickCallback,
    children
}): React.ReactNode => {

    const [acceptedFileTypes, setAcceptedFileTypes] = React.useState<string>();

    const onUpload = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        
        if(file && onClickCallback) {
            onClickCallback(file);
        }
    };

    const setAcceptedFileTypesHandler = () => {
        switch(fileType) {
            case 'image':
                setAcceptedFileTypes('image/png, image/jpeg, image/jpg');
                break;
            case 'video':
                setAcceptedFileTypes('video/mp4, video/avi, video/mov');
                break;
            case 'audio':
                setAcceptedFileTypes('audio/mpeg, audio/wav');
                break;
            case 'document':
                setAcceptedFileTypes('.pdf, .doc, .docx, .xls, .xlsx');
                break;
            default:
                setAcceptedFileTypes('*/*');
        }
    };
   
    useEffect(() => {
        setAcceptedFileTypesHandler();
    }, []);

    return (
        <>
            <label htmlFor={`upload_${fileType}`}  className={`${customStyle}`}>{children}</label>
            <input
                multiple={false}
                type="file"
                accept={acceptedFileTypes}
                id={`upload_${fileType}`}
                onChange={onUpload}
                style={{
                    display: 'none',
                }}
            />
        </>
    );
};

export default CustomUpload;