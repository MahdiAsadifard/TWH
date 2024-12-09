import {
    useId,
    Link,
    Toaster,
    useToastController,
    Toast,
    ToastTitle,
    ToastBody,
    ToastFooter
} from "@fluentui/react-components";

interface IProps {
    title?: string;
    titleAction?: any; //<Link>Custom Link</Link>
    subtitle?: string;
    body?: string;
    footerActions?: { label: string; onClick: ()=> void|Promise<void>; }[] ;
};

const UseCustomToast = () => {

    const toasterId = useId("toaster");
    const { dispatchToast } = useToastController(toasterId);

    const showToast = ({
        title,
        titleAction,
        subtitle,
        body,
        footerActions,
    }: IProps) => {
        dispatchToast(
            <Toast>
                <ToastTitle action={titleAction}>{title}</ToastTitle>
                <ToastBody subtitle={subtitle}>{body}</ToastBody>
                {footerActions && (
                    <ToastFooter>
                        {
                            footerActions.map((f, i) => {
                                return <Link key={i} onClick={f.onClick}>{f.label}</Link>
                            })
                        }
                    </ToastFooter>
                )}
            </Toast>
            , {
                position: 'top'
            }
        );
    };

    return {
        showToast,
        ToasterComponent: () => <Toaster toasterId={toasterId} />
    };
};

export default UseCustomToast;