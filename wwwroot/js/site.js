$(document).ready(function () {
    var SPMaskBehavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    },
        spOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(SPMaskBehavior.apply({}, arguments), options);
            }
        };

    $('.sp_celphones').mask(SPMaskBehavior, spOptions);

    $('.cpf').mask('000.000.000-00');
    $('.cep').mask('00000-000');

    $('#PostalCode').on('blur', function () {
        var cep = $(this).val().replace(/\D/g, '');

        if (cep.length === 8) {
            $.getJSON(`https://viacep.com.br/ws/${cep}/json/`, function (data) {
                if (!("erro" in data)) {
                    $('#Address').val(data.logradouro);
                    $('#City').val(data.localidade);
                    $('#State').val(data.uf);
                } else {
                    Swal.fire({
                        title: "CEP não encontrado",
                        text: "Por favor, verifique o CEP digitado e tente novamente.",
                        icon: "error"
                    });

                    $('#Address').val();
                    $('#City').val();
                    $('#State').val();
                }
            }).fail(function () {
                Swal.fire({
                    title: "Erro",
                    text: "Erro ao buscar o CEP. Tente novamente mais tarde.",
                    icon: "error"
                });
            });
        } else {
            Swal.fire({
                title: "CEP Inválido",
                text: "Por favor, digite um CEP válido.",
                icon: "error"
            });
        }
    });

    if ($('#validation-container ul').length > 0) {
        $('#validation-container-parent').removeClass('d-none');

        setTimeout(function () {
            $('#validation-container').fadeOut();
        }, 2000);
    }
    else{
        $('#validation-container-parent').addClass('d-none');
    }
});

function deleteAddress(addressId) {
    var personId = $('#PersonId').val();

    $.ajax({
        url: `/Home/DeleteAddress/?id=${addressId}&personId=${personId}`,
        type: 'DELETE',
        success: function (result) {
            window.location.href = `/Home/Dados/${personId}`;
        },
        error: function (xhr, status, error) {
            Swal.fire({
                title: "Erro ao deletar o endereço",
                text: "Por favor, tente novamente mais tarde.",
                icon: "error"
            });
        }
    });
}

function deletePerson(id) {
    $.ajax({
        url: `/Person/DeletePerson/${id}`,
        type: 'DELETE',
        success: function (result) {
            window.location.href = `/Home`;
        },
        error: function (xhr, status, error) {
            Swal.fire({
                title: "Erro ao deletar o usuário",
                text: "Por favor, tente novamente mais tarde.",
                icon: "error"
            });
        }
    });
}