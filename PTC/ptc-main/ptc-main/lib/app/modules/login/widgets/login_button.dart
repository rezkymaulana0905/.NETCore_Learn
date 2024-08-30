import '../../../constants/constants.dart';
import '../../../widgets/widgets.dart';

class LoginButton extends StatelessWidget {
  const LoginButton({
    super.key,
    required this.onPressed,
    required this.isLoading,
    required this.text,
  });

  final VoidCallback onPressed;
  final RxBool isLoading;
  final String text;

  @override
  Widget build(BuildContext context) {
    return ConfirmButton(
      text: text,
      onPressed: () => onPressed(),
      isLoading: isLoading,
      color: AppColor.theme,
      width: baseWidth * 0.6,
      height: baseHeight * 0.08,
      skipDialog: true,
    );
  }
}
