import '../constants/constants.dart';

abstract class _MyForm extends StatelessWidget {
  const _MyForm({super.key, this.size, this.border, required this.form});

  final Size? size;
  final Border? border;
  final Map<String,dynamic> form;

  Size _getSize(Orientation orientation) {
    return switch (orientation) {
      Orientation.portrait => Size(baseWidth * 0.4, baseHeight * 0.06),
      Orientation.landscape => Size(baseWidth * 0.35, baseHeight * 0.05),
    };
  }

  @override
  Widget build(BuildContext context) {
    return Builder(builder: (context) {
      final defaultSize = _getSize(context.orientation);
      return SizedBox(
        width: size?.width ?? defaultSize.width,
        child: Padding(
          padding: EdgeInsets.symmetric(vertical: baseHeight * 0.005),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: <Widget>[
              if (form['title'] != null) _buildTitle(),
              Container(
                height: size?.height ?? defaultSize.height,
                padding: const EdgeInsets.all(8),
                alignment: Alignment.centerLeft,
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.circular(10),
                  border: border,
                  boxShadow: const <BoxShadow>[
                    BoxShadow(
                      color: AppColor.shadow,
                      blurRadius: 1,
                      spreadRadius: 2,
                    ),
                  ],
                ),
                child: _buildForm(),
              ),
            ],
          ),
        ),
      );
    });
  }

  Widget _buildTitle() {
    return Padding(
      padding: const EdgeInsets.only(left: 5, bottom: 5),
      child: FittedBox(
        fit: BoxFit.scaleDown,
        child: Text(
          form['title'],
          style: TextStyle(
            fontWeight: FontWeight.w600,
            fontSize: baseWidth * 0.025,
          ),
        ),
      ),
    );
  }

  Widget _buildForm();
}

class FormValue extends _MyForm {
  const FormValue({super.key, required super.form});

  @override
  Widget _buildForm() {
    return FittedBox(
      child: Text(
        "${form['value'] ?? "-"}",
        maxLines: 1,
        style: TextStyle(fontSize: baseHeight * 0.015),
      ),
    );
  }
}

class FormInput extends _MyForm {
  FormInput({
    super.key,
    super.size,
    super.border,
    required super.form,
    this.hintText,
    this.icon,
    this.value,
    this.isPasswordForm = false,
  }) : isObscure = isPasswordForm.obs;

  final String? hintText;
  final Icon? icon;
  final dynamic value;
  final bool isPasswordForm;
  final RxBool isObscure;

  @override
  Widget _buildForm() {
    return Obx(
      () => TextField(
        controller: form['controller'],
        inputFormatters: [
          LengthLimitingTextInputFormatter(form['length'] ?? 50),
          if (form['pattern'] != null)
            FilteringTextInputFormatter.allow(RegExp(form['pattern']!)),
          if (form['formType'] == TextInputType.number)
            FilteringTextInputFormatter.allow(RegExp(r'[1-9]')),
          if (form['textCase'] == TextCapitalization.characters)
            FilteringTextInputFormatter.allow(RegExp(r'[A-Z0-9\s]')),
        ],
        keyboardType: form['formType'] ?? TextInputType.text,
        obscureText: isObscure.value,
        textCapitalization: form['textCase'] ?? TextCapitalization.none,
        decoration: InputDecoration(
          border: InputBorder.none,
          hintText: hintText ?? "Enter ${form['title']}",
          hintStyle: const TextStyle(fontSize: 14, color: AppColor.shadow),
          prefixIcon: icon,
          suffixIcon: isPasswordForm
              ? IconButton(
                  icon: Icon(
                    isObscure.value ? Icons.visibility_off : Icons.visibility,
                    color: AppColor.off,
                  ),
                  onPressed: () => isObscure.value = !isObscure.value,
                )
              : null,
        ),
      ),
    );
  }
}
