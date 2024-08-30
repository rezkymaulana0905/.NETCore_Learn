import '../../../constants/constants.dart';
import '../../../widgets/widgets.dart';

class LoginForm extends StatelessWidget {
  const LoginForm({
    super.key,
    required this.hintText,
    required this.icon,
    required this.textController,
    this.isPasswordForm = false,
  });

  final String hintText;
  final IconData icon;
  final TextEditingController textController;
  final bool isPasswordForm;

  @override
  Widget build(BuildContext context) {
    return FormInput(
      size: Size(baseWidth * 0.6, baseHeight * 0.08),
      border: Border.all(color: AppColor.theme, width: 3),
      form: {'controller': textController},
      hintText: hintText,
      icon: Icon(icon, color: AppColor.shadow),
      isPasswordForm: isPasswordForm,
    );
  }
}
